export default function Input(props: any) {
  return (
    <div className="flex flex-col py-2">
      <label htmlFor={props.name} className="pb-2">{props?.label}</label>
      {
        props.as == 'textarea' ?
          <textarea {...props} className="border border-gray-300 rounded-md px-4 py-2"></textarea>
        : <input {...props} className="border border-gray-300 rounded-md px-4 py-2"/>
      }
    </div>
  );
}
